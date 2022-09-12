<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Sabre_QueueRQ.xsl 																-->
<!-- ================================================================== -->
<!-- Date: 10 Oct 2013 - Rastko - new file												-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_QueueRQ" />
	</xsl:template>
	
	<xsl:template match="OTA_QueueRQ">
		<xsl:apply-templates select="ListQueue"/>
		<xsl:apply-templates select="CountQueue"/>
		<xsl:apply-templates select="RemoveQueue"/>
		<xsl:apply-templates select="BounceQueue"/>
		<xsl:apply-templates select="Move"/>
		<xsl:apply-templates select="PlaceQueue"/>
	</xsl:template>
	
	<xsl:template match="PlaceQueue">
		<QueuePlaceRQ ReturnHostCommand="false" TimeStamp="2012-09-18T11:15:00-06:00" Version="2.0.2" xmlns="http://webservices.sabre.com/sabreXML/2011/10" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
			<QueueInfo>
				<QueueIdentifier>
					<xsl:choose>
						<xsl:when test="@Number != ''">
							<xsl:attribute name="Number"><xsl:value-of select="@Number"/></xsl:attribute>
						</xsl:when>
						<xsl:when test="@Name != ''">
							<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
						</xsl:when>
					</xsl:choose>
					<xsl:choose>
						<xsl:when test="@Category!=''">
							<xsl:attribute name="PrefatoryInstructionCode"><xsl:value-of select="@Category"/></xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="PrefatoryInstructionCode"><xsl:value-of select="'0'"/></xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:if test="@PseudoCityCode != ''">
						<xsl:attribute name="PseudoCityCode">
							<xsl:value-of select="@PseudoCityCode"/>
						</xsl:attribute> 
					</xsl:if>
				</QueueIdentifier>
				<UniqueID>
					<xsl:attribute name="ID"><xsl:value-of select="UniqueID/@ID"/></xsl:attribute>
				</UniqueID>
			</QueueInfo>
		</QueuePlaceRQ>
	</xsl:template>
	
	<xsl:template match="BounceQueue">	 
		<SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.6.1">
			<xsl:element name="Request">
				<xsl:attribute name="Output">SCREEN</xsl:attribute>
				<xsl:attribute name="MDRSubset">AD01</xsl:attribute>
				<xsl:attribute name="CDATA">true</xsl:attribute>
				<xsl:element name="HostCommand">
					<xsl:choose>
						<xsl:when test="@Action='R'">
							<xsl:text>QBR‡1</xsl:text>
						</xsl:when>
						<xsl:when test="@Action='I'">
							<xsl:text>QBI‡1</xsl:text>
						</xsl:when>
						<xsl:when test="@Action='E'">
							<xsl:text>QBE‡1</xsl:text>
						</xsl:when>
					</xsl:choose> 
				</xsl:element>
			</xsl:element>
		</SabreCommandLLSRQ>  
   	</xsl:template>

	<xsl:template match="RemoveQueue">
		<SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.6.1">
			<xsl:element name="Request">
				<xsl:attribute name="Output">SCREEN</xsl:attribute>
				<xsl:attribute name="MDRSubset">AD01</xsl:attribute>
				<xsl:attribute name="CDATA">true</xsl:attribute>
				<xsl:element name="HostCommand">
          <!-- QR/‡ -->
					<xsl:text>QR/</xsl:text>
					<xsl:if test="@PseudoCityCode != ''">
						<xsl:value-of select="@PseudoCityCode"/>
					</xsl:if>
					<xsl:value-of select="@Number"/>
				</xsl:element>
			</xsl:element>
		</SabreCommandLLSRQ>
	</xsl:template>
	
	<xsl:template match="Move">
		<QMoveRQ TimeStamp="2010-11-30T16:00:00-06:00" Version="1.0.1" xmlns="http://webservices.sabre.com/sabreXML/2003/07" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
			<!--POS>
				<Source PseudoCityCode="IPCC"/>
			</POS-->
			<QInfo>
				<OriginQueue>
					<QueueIdentifier>
						<xsl:attribute name="Number">
							<xsl:value-of select="FromQueue/@Number"/>
						</xsl:attribute>
					</QueueIdentifier>
				</OriginQueue>
				<DestinationQueue>
					<QueueIdentifier>
						<xsl:attribute name="Number">
							<xsl:value-of select="ToQueue/@Number"/>
						</xsl:attribute>
					</QueueIdentifier>
				</DestinationQueue>
			</QInfo>
		</QMoveRQ>
	</xsl:template>
	
	<xsl:template match="ListQueue">
		<SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.6.1">
			<xsl:element name="Request">
				<xsl:attribute name="Output">SCREEN</xsl:attribute>
				<xsl:attribute name="MDRSubset">AD01</xsl:attribute>
				<xsl:attribute name="CDATA">true</xsl:attribute>
				<xsl:element name="HostCommand">
					<xsl:text>Q/</xsl:text>
					<xsl:if test="@PseudoCityCode != ''">
						<xsl:value-of select="@PseudoCityCode"/>
					</xsl:if>
					<xsl:value-of select="@Number"/>
					<xsl:text>/L</xsl:text>
				</xsl:element>
			</xsl:element>
		</SabreCommandLLSRQ>
	</xsl:template>
	
	<xsl:template match="CountQueue">
		<QueueCountRQ xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.0.0">
			<QueueInfo>
				<QueueIdentifier>
					<xsl:if test="@PseudoCityCode != ''">
						<xsl:attribute name="PseudoCityCode"><xsl:value-of select="@PseudoCityCode"/></xsl:attribute>
					</xsl:if>
					<xsl:if test="@Number !=''">
						<xsl:attribute name="Number"><xsl:value-of select="@Number"/></xsl:attribute>
					</xsl:if>
				</QueueIdentifier>
			</QueueInfo>
		</QueueCountRQ>
	</xsl:template>
	
</xsl:stylesheet>
