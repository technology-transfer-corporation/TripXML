<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<TTWSAirlines>
		<xsl:variable name="airlines">
			<xsl:apply-templates select="TTWSAirlines/Airline" mode="thomalex">
				<xsl:sort order="ascending" data-type="text" select="Code"/>
			</xsl:apply-templates>
			<xsl:apply-templates select="TTWSAirlines/AirlineNamesOAG/Airline" mode="jetcombo">
				<xsl:sort order="ascending" data-type="text" select="@Code"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:apply-templates select="msxsl:node-set($airlines)/Airline" mode="all">
			<xsl:sort order="ascending" data-type="text" select="Code"/>
		</xsl:apply-templates>
	</TTWSAirlines>
</xsl:template>

<xsl:template match="Airline" mode="thomalex">
	<Airline>
		<Code><xsl:value-of select="Code"/></Code>
		<xsl:variable name="code"><xsl:value-of select="Code"/></xsl:variable>
		<Name>
			<xsl:choose>
				<xsl:when test="../AirlineNamesOAG/Airline[@Code=$code]">
					<xsl:value-of select="../AirlineNamesOAG/Airline[@Code=$code]/@OAGName"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="name"><xsl:value-of select="Name"/></xsl:variable>
					<xsl:call-template name="buildName">
						<xsl:with-param name="name"><xsl:value-of select="$name"/></xsl:with-param>
						<xsl:with-param name="finalname"></xsl:with-param>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</Name>
	</Airline>
</xsl:template>  

<xsl:template match="Airline" mode="jetcombo">
	<xsl:variable name="code"><xsl:value-of select="@Code"/></xsl:variable>
	<xsl:choose>
		<xsl:when test="../../Airline[Code=$code] or string-length(@Code)!=2"></xsl:when>
		<xsl:otherwise>
			<Airline>
				<Code><xsl:value-of select="@Code"/></Code>
				<Name><xsl:value-of select="@OAGName"/></Name>
			</Airline>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="Airline" mode="all">
	<xsl:copy-of select="."/>
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