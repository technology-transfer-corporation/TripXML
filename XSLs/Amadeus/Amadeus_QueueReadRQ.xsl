<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Amadeus_QueueReadRQ.xsl 						 								       -->
<!-- ================================================================== -->
<!-- Date: 25 Sep 2008  - Rastko													       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_QueueReadRQ" />
	</xsl:template>
	
	<xsl:template match="OTA_QueueReadRQ">
		<xsl:apply-templates select="ItemOnQueue"/>
		<xsl:apply-templates select="AccessQueue"/>
		<xsl:apply-templates select="ExitQueue"/>
	</xsl:template>

	<xsl:template match="ItemOnQueue">
		<xsl:choose>
			<xsl:when test="@Action = 'NextRemove'">
				<Cryptic_GetScreen_Query>
					<Command>QN</Command>
				</Cryptic_GetScreen_Query>
			</xsl:when>
                <xsl:when test="@Action = 'NextKeep'">
				<Cryptic_GetScreen_Query>
					<Command>IG</Command>
				</Cryptic_GetScreen_Query>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="ExitQueue">
		<Queue_IgnoreItem_Query/>
	</xsl:template>
	
	<xsl:template match="AccessQueue">
		<Queue_Start_Query>
			<Queue><xsl:value-of select="@Number"/></Queue>
			<xsl:if test="@Category != ''">
				<Category><xsl:value-of select="@Category"/></Category>
			</xsl:if>
			<xsl:if test="@PseudoCityCode != ''">
				<OfficeID><xsl:value-of select="@PseudoCityCode"/></OfficeID>
			</xsl:if>
		</Queue_Start_Query>	
	</xsl:template>
		
</xsl:stylesheet>
