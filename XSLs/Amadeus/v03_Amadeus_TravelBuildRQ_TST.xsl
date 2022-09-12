<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_LowFareFlights1RS.xsl 											-->
	<!-- ================================================================== -->
	<!-- Date: 05 Apr 2011 - Sajith - hard coded B6 as OSI airline						-->
	<!-- Date: 18 Mar 2011 - Sajith - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="PoweredTicket_DisplayTSTReply"/>
	</xsl:template>
	<xsl:template match="PoweredTicket_DisplayTSTReply">
		<PoweredPNR_AddMultiElements>
			  <pnrActions> 
		       	   <optionCode>10</optionCode> 
			   </pnrActions> 
		     <dataElementsMaster> 
	          	<marker1>1</marker1>
	          	<xsl:for-each select="fareList">
		          	<xsl:variable name="fares">
		          		<xsl:value-of select="'FARE'"/>
		          		<xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier='B']/fareAmount"/>
		          		<xsl:value-of select="'/TAXES '"/>
		          		<xsl:for-each select="taxInformation">
		          			<xsl:value-of select="amountDetails/fareDataMainInformation/fareAmount"/>
		          			<xsl:value-of select="concat(taxDetails/taxType/isoCountry,'/')"/>
		          		</xsl:for-each>
		          		<xsl:value-of select="'TOTAL '"/>
		          		<xsl:value-of select="fareDataInformation/fareDataSupInformation[fareDataQualifier='TFT']/fareAmount"/>
		          	</xsl:variable>
		          	<xsl:call-template name="OSI">
		          		<xsl:with-param name="airlineCode" select="'B6'"/>
						<xsl:with-param name="osiText"><xsl:value-of select="$fares"/></xsl:with-param>
					</xsl:call-template>
					<xsl:call-template name="OSI">
						<xsl:with-param name="airlineCode" select="'B6'"/>
						<xsl:with-param name="osiText" select="otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription"/>
					</xsl:call-template>
				</xsl:for-each>
				<dataElementsIndiv>
					<elementManagementData>
						<segmentName>RF</segmentName>
					</elementManagementData>
					<freetextData>
						<freetextDetail>
							<subjectQualifier>3</subjectQualifier>
							<type>P22</type>
						</freetextDetail>
						<longFreetext>TRIPXML</longFreetext>
					</freetextData>
				</dataElementsIndiv>
			</dataElementsMaster> 
		</PoweredPNR_AddMultiElements>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		       OSI			 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template name="OSI">
		<xsl:param name="airlineCode"/>
		<!-- has to be same names as calling parameters -->
		<xsl:param name="osiText"/>
		<xsl:if test="string-length($osiText) > 0">
			<xsl:variable name="osi60">
				<xsl:choose>
					<xsl:when test="string-length($osiText) > 60">
						<xsl:value-of select="substring($osiText,1,60)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$osiText"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="osiMore">
				<xsl:if test="string-length($osiText) > 60">
					<xsl:value-of select="substring($osiText,61)"/>
				</xsl:if>
			</xsl:variable>
			<dataElementsIndiv>
				<elementManagementData>
					<segmentName>OS</segmentName>
				</elementManagementData>
				<freetextData>
					<freetextDetail>
						<subjectQualifier>3</subjectQualifier>
						<type>28</type>
						<companyId>
							<xsl:value-of select="$airlineCode"/>
						</companyId>
					</freetextDetail>
					<longFreetext>
						<xsl:value-of select="$osi60"/>
					</longFreetext>
				</freetextData>
			</dataElementsIndiv>
			<xsl:call-template name="OSI">
				<xsl:with-param name="airlineCode" select="$airlineCode"/>
				<xsl:with-param name="osiText" select="$osiMore"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
