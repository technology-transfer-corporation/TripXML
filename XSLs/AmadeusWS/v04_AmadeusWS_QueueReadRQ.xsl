<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- v04_AmadeusWS_QueueReadRQ.xsl 											       -->
<!-- ================================================================== -->
<!-- Date: 10 Jan 2011  - Rastko														       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_QueueReadRQ" />
	</xsl:template>
	
	<xsl:template match="OTA_QueueReadRQ">
		<xsl:apply-templates select="ItemOnQueue[@Action = 'NextRemove' or @Action = 'NextKeep']" mode="next"/>
		<xsl:apply-templates select="ItemOnQueue[@Action = 'Redisplay']" mode="redisplay"/>
		<xsl:apply-templates select="AccessQueue"/>
		<xsl:apply-templates select="ExitQueue"/>
	</xsl:template>

	<xsl:template match="ItemOnQueue" mode="next">
		<Command_Cryptic>
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<textStringDetails>
					<xsl:choose>
						<xsl:when test="@Action = 'NextRemove'">
								<xsl:text>QN</xsl:text>
						</xsl:when>
			                <xsl:when test="@Action = 'NextKeep'">
								<xsl:text>IG</xsl:text>
						</xsl:when>
					</xsl:choose>
				</textStringDetails>
			</longTextString>
		</Command_Cryptic>
	</xsl:template>
	
	<xsl:template match="ItemOnQueue" mode="redisplay">
		<PNR_Retrieve>
			<settings>
				<options>
					<optionCode>51</optionCode>
				</options>
			</settings>
			<retrievalFacts>
				<retrieve>
					<type>1</type>
				</retrieve>
			</retrievalFacts>
		</PNR_Retrieve>
	</xsl:template>
	
	<xsl:template match="ExitQueue">
		<Command_Cryptic>
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<textStringDetails>
					<xsl:text>QI</xsl:text>
				</textStringDetails>
			</longTextString>
		</Command_Cryptic>
	</xsl:template>
	
	<xsl:template match="AccessQueue">
		<Command_Cryptic>
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<textStringDetails>
					<xsl:text>QS</xsl:text>
					<xsl:if test="@PseudoCityCode != ''">
						<xsl:text>/</xsl:text>
						<xsl:value-of select="@PseudoCityCode"/>
					</xsl:if>
					<xsl:text>/</xsl:text>
					<xsl:value-of select="@Number"/>
					<xsl:if test="@Category != ''">
						<xsl:text>C</xsl:text>
						<xsl:value-of select="@Category"/>
					</xsl:if>
				</textStringDetails>
			</longTextString>
		</Command_Cryptic>
	</xsl:template>
		
</xsl:stylesheet>
