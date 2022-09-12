<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseCabinHoldRQ.xsl 	     									       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" indent="yes"/>

<xsl:template match="OTA_CruiseCabinUnholdRQ">
	<Cruise_UnholdCabin>
		<xsl:element name="processingInfo">
			<xsl:element name="processingDetails">
				<xsl:element name="businessType">
					<xsl:value-of select="'5'"/>
				</xsl:element>
				<xsl:element name="function">
					<xsl:value-of select="'56'"/>
				</xsl:element>
			</xsl:element>
		</xsl:element>
		<agentEnvironment>
			<agentTerminalId>
				<xsl:value-of select="'12345678'"/>
			</agentTerminalId>
		</agentEnvironment>
		<sailingGroup>
			<sailingDescription>
				<providerDetails>
					<shipCode>
						<xsl:value-of select="SelectedSailing/@ShipCode"/>
					</shipCode>
					<cruiselineCode>
						<xsl:value-of select="SelectedSailing/@VendorCode"/>
					</cruiselineCode>
				</providerDetails>
				<sailingDateTime>
					<sailingDepartureDate>
						<xsl:value-of select="substring(SelectedSailing/@Start,9,2)"/>
						<xsl:value-of select="substring(SelectedSailing/@Start,6,2)"/>
						<xsl:value-of select="substring(SelectedSailing/@Start,1,4)"/>
					</sailingDepartureDate>
					<sailingDuration>
						<xsl:value-of select="SelectedSailing/@Duration"/>
					</sailingDuration>
				</sailingDateTime>
				<xsl:apply-templates select="SelectedSailing/@VoyageID"/>
			</sailingDescription>
			<xsl:apply-templates select="SelectedSailing/SelectedCabin"/>
		</sailingGroup>		
	</Cruise_UnholdCabin>
</xsl:template>

<xsl:template match="@VoyageID">
	<sailingId>
		<cruiseVoyageNbr>
			<xsl:value-of select="."/>
		</cruiseVoyageNbr>
	</sailingId>
</xsl:template>

<xsl:template match="SelectedCabin">
	<cabinInfo>
		<cabinDetails>
			<cabinNbr>
				<xsl:value-of select="@CabinNumber"/>
			</cabinNbr>						
		</cabinDetails>
	</cabinInfo>
</xsl:template>


</xsl:stylesheet>
