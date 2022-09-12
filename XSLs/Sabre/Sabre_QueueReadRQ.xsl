<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- 
==================================================================
Sabre_QueueReadRQ.xsl 															       
================================================================== 
Date: 02 Jun 2021  - Kobelev - Upgraded SabreCommandLLSRQ to version 2.0
Date: 16 Apr 2014  - Rastko - added VerifyTickets option						
Date: 10 Oct 2013  - Rastko - new file											       
================================================================== 
-->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_QueueReadRQ" />
	</xsl:template>
	
	<xsl:template match="OTA_QueueReadRQ">
		<QueueRead>
			<xsl:apply-templates select="ItemOnQueue[@Action = 'NextRemove' or @Action = 'NextKeep']" mode="next"/>
			<xsl:apply-templates select="ItemOnQueue[@Action = 'Redisplay']" mode="redisplay"/>
			<xsl:apply-templates select="AccessQueue"/>
			<xsl:apply-templates select="ExitQueue"/>
			<xsl:apply-templates select="VerifyTickets"/>
		</QueueRead>
	</xsl:template>

	<xsl:template match="ItemOnQueue" mode="next">
		<SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.0.0">
			<xsl:element name="Request">
				<xsl:attribute name="Output">SCREEN</xsl:attribute>
				<xsl:attribute name="MDRSubset">AD01</xsl:attribute>
				<xsl:attribute name="CDATA">true</xsl:attribute>
				<xsl:element name="HostCommand">
					<xsl:choose>
						<xsl:when test="@Action = 'NextRemove'">
								<xsl:text>QBR'1</xsl:text>
						</xsl:when>
			                <xsl:when test="@Action = 'NextKeep'">
								<xsl:text>QBI'1</xsl:text>
						</xsl:when>
					</xsl:choose>
				</xsl:element>
			</xsl:element>
		</SabreCommandLLSRQ>
	</xsl:template>
	
	<xsl:template match="ItemOnQueue" mode="redisplay">
		<TravelItineraryReadRQ Version="2.0.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
			<MessagingDetails>
				<Transaction  Code="PNR" /> 
			</MessagingDetails>
		</TravelItineraryReadRQ>
	</xsl:template>
	
	<xsl:template match="ExitQueue">
		<SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.0.0">
			<xsl:element name="Request">
				<xsl:attribute name="Output">SCREEN</xsl:attribute>
				<xsl:attribute name="MDRSubset">AD01</xsl:attribute>
				<xsl:attribute name="CDATA">true</xsl:attribute>
				<xsl:element name="HostCommand">QXI</xsl:element>
			</xsl:element>
		</SabreCommandLLSRQ>
	</xsl:template>
	
	<xsl:template match="AccessQueue">
		<QueueAccessRQ Version="2.0.2" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
			<QueueIdentifier>
				<xsl:attribute name="Number"><xsl:value-of select="@Number"/></xsl:attribute>
				<xsl:if test="@PseudoCityCode != ''">
					<xsl:attribute name="PseudoCityCode"><xsl:value-of select="@PseudoCityCode"/></xsl:attribute>
				</xsl:if>
			</QueueIdentifier> 
		</QueueAccessRQ>
	</xsl:template>
	
	<xsl:template match="VerifyTickets">
		<VerifyTickets>
			<xsl:for-each select="TicketDate">
				<xsl:variable name="dt" select="."/>
				<xsl:for-each select="../PseudoCityCode">
					<DailySalesReportRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.0.0">
						<SalesReport StartDate="{substring($dt,1,10)}" PseudoCityCode="{.}" />
					</DailySalesReportRQ>
				</xsl:for-each>
			</xsl:for-each>
		</VerifyTickets>
	</xsl:template>
		
</xsl:stylesheet>
