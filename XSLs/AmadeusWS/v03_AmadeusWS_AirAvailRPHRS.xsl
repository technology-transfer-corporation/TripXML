<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ttVB="urn:ttVB" exclude-result-prefixes="msxsl ttVB" version="1.0">
	<!-- ================================================================== -->
	<!-- v03_Amadeusws_AirAvailRPHRS.xsl												-->
	<!-- ================================================================== -->
	<!-- Date: 05 Jan 2011 - Rastko - completed mapping of all flight data				-->
	<!-- Date: 24 Mar 2006 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirAvailRS" />
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_AirAvailRS">
		<OTA_AirAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
			<Success></Success>
			<OriginDestinationOptions>
				<xsl:apply-templates select="//OriginDestinationOption">
					<xsl:sort data-type="text" order="ascending" select="FlightSegment[1]/@DepartureDateTime"/>
				</xsl:apply-templates>
			</OriginDestinationOptions>
		</OTA_AirAvailRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="OriginDestinationOption">
		<OriginDestinationOption>
			
			<xsl:variable name ="rph"><xsl:value-of select="position()"/></xsl:variable>
			<xsl:for-each select="TPA_Extensions">
			<TPA_Extensions>
				<xsl:copy-of select="TotalJourneyDuration" />
			</TPA_Extensions>
			</xsl:for-each>
			<xsl:for-each select="FlightSegment">
			
			<FlightSegment>
				<xsl:attribute name="DepartureDateTime"><xsl:value-of select="@DepartureDateTime"/></xsl:attribute>
				<xsl:attribute name="ArrivalDateTime"><xsl:value-of select="@ArrivalDateTime"/></xsl:attribute>
				<xsl:attribute name="StopQuantity"><xsl:value-of select="@StopQuantity"/></xsl:attribute>
				<xsl:attribute name="StopQuantity"><xsl:value-of select="@StopQuantity"/></xsl:attribute>
				<xsl:attribute name="RPH"><xsl:value-of select="$rph"/></xsl:attribute>
				<xsl:attribute name="FlightNumber"><xsl:value-of select="@FlightNumber"/></xsl:attribute>
				<xsl:attribute name="Ticket"><xsl:value-of select="@Ticket"/></xsl:attribute>
				<xsl:attribute name="StopQuantity"><xsl:value-of select="@StopQuantity"/></xsl:attribute>
				<xsl:copy-of select="DepartureAirport" />
				<xsl:copy-of select="ArrivalAirport" />
				<xsl:copy-of select="OperatingAirline" />
				<xsl:copy-of select="Equipment" />			
				<xsl:copy-of select="MarketingCabin" />	
				<xsl:copy-of select="MarketingAirline" />
				<xsl:copy-of select="BookingClassAvail"/>
				<xsl:copy-of select="TPA_Extensions"/>
			</FlightSegment>
			
			</xsl:for-each>
		</OriginDestinationOption>
	</xsl:template>
	<!--*************************************************************-->
	
</xsl:stylesheet>
<!-- Stylus Studio meta-information - (c) 2004-2009. Progress Software Corporation. All rights reserved.

<metaInformation>
	<scenarios>
		<scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="res31.xml" htmlbaseurl="" outputurl="test2.xml" processortype="saxon8" useresolver="yes" profilemode="0" profiledepth="" profilelength="" urlprofilexml=""
		          commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="">
			<advancedProp name="sInitialMode" value=""/>
			<advancedProp name="bXsltOneIsOkay" value="true"/>
			<advancedProp name="bSchemaAware" value="true"/>
			<advancedProp name="bXml11" value="false"/>
			<advancedProp name="iValidation" value="0"/>
			<advancedProp name="bExtensions" value="true"/>
			<advancedProp name="iWhitespace" value="0"/>
			<advancedProp name="sInitialTemplate" value=""/>
			<advancedProp name="bTinyTree" value="true"/>
			<advancedProp name="bWarnings" value="true"/>
			<advancedProp name="bUseDTD" value="false"/>
			<advancedProp name="iErrorHandling" value="fatal"/>
		</scenario>
	</scenarios>
	<MapperMetaTag>
		<MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/>
		<MapperBlockPosition></MapperBlockPosition>
		<TemplateContext></TemplateContext>
		<MapperFilter side="source"></MapperFilter>
	</MapperMetaTag>
</metaInformation>
-->