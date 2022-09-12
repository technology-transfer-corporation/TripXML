<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Galileo_ETicketVerifyRQ.xsl 															-->
<!-- ================================================================== -->
<!-- Date: 08 Jan 2007 - Rastko															-->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<ETkt>
		<PNRBFManagement_7_9>
			<xsl:apply-templates select="OTA_ETicketVerifyRQ" />
		</PNRBFManagement_7_9>
		<DocProdETicketCheck_1_0>
			<EligibilityMods>
				<AssocSegs>
					<SegNumAry>
						<xsl:apply-templates select="OTA_ETicketVerifyRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment" mode="pos"/>
					</SegNumAry>
				</AssocSegs>
			</EligibilityMods>
		</DocProdETicketCheck_1_0>
		<PNRBFManagement_7_9>
			<IgnoreMods/> 
		</PNRBFManagement_7_9>
	</ETkt>
</xsl:template>

<xsl:template match="OTA_ETicketVerifyRQ">
	<AirSegSellMods>
		<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption" mode="fs" />
	</AirSegSellMods>
</xsl:template>

<xsl:template match="OriginDestinationOption" mode="fs">
	<xsl:variable name="od"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:variable name="numfl"><xsl:value-of select="count(FlightSegment)"/></xsl:variable>
	<xsl:apply-templates select="FlightSegment" mode="fs">
		<xsl:with-param name="od"><xsl:value-of select="$od"/></xsl:with-param>
		<xsl:with-param name="numfl"><xsl:value-of select="$numfl"/></xsl:with-param>
	</xsl:apply-templates>
</xsl:template>
	
<xsl:template match="FlightSegment" mode="fs">
	<xsl:param name="od"/>
	<xsl:param name="numfl"/>
	<AirSegSell>
		<Vnd>
			<xsl:value-of select="MarketingAirline/@Code" />
			<xsl:text><![CDATA[ ]]></xsl:text>
		</Vnd>
		<FltNum>
			<xsl:value-of select="@FlightNumber" />
		</FltNum>
		<OpSuf><![CDATA[ ]]></OpSuf>
		<Class>
			<xsl:value-of select="@ResBookDesigCode" />
			<xsl:text><![CDATA[ ]]></xsl:text>
		</Class>
		<StartDt>
			<xsl:value-of select="substring(@DepartureDateTime,1,4)" />
			<xsl:value-of select="substring(@DepartureDateTime,6,2)" />
			<xsl:value-of select="substring(@DepartureDateTime,9,2)" />
		</StartDt>
		<StartAirp>
			<xsl:value-of select="DepartureAirport/@LocationCode" />
			<xsl:text><![CDATA[  ]]></xsl:text>
		</StartAirp>
		<EndAirp>
			<xsl:value-of select="ArrivalAirport/@LocationCode" />
			<xsl:text><![CDATA[  ]]></xsl:text>
		</EndAirp>
		<Status>NN</Status>
		<NumPsgrs>1</NumPsgrs>
		<StartTm><xsl:value-of select="translate(substring(@DepartureDateTime,12,5),':','')"></xsl:value-of></StartTm>
		<EndTm><xsl:value-of select="translate(substring(@ArrivalDateTime,12,5),':','')"></xsl:value-of></EndTm>
		<DtChg>00</DtChg>
		<StopoverIgnoreInd />
		<AvailDispType>G</AvailDispType>
		<VSpec />
		<AvailJrnyNum>
			<xsl:if test="$numfl > 1">
				<xsl:value-of select="format-number($od,'00')"/>
			</xsl:if>
		</AvailJrnyNum>
	</AirSegSell>
</xsl:template>
	
<xsl:template match="FlightSegment" mode="pos">
	<SegNum>
		<xsl:text>00</xsl:text>
		<xsl:value-of select="position()"/>
	</SegNum> 
</xsl:template>
	
</xsl:stylesheet>



