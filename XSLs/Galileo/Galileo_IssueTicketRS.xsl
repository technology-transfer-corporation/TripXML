<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_IssueTicketRS.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 28 Jul 2012 - Rastko - added error text when none returned by Galileo			-->
	<!-- Date: 09 Mar 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<TT_IssueTicketRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
			<xsl:apply-templates select="DocProdFareManipulation_10" />
      <xsl:apply-templates select="DocProdFareManipulation_29" />
			<xsl:apply-templates select="PNRBFManagement_53" />
			<xsl:apply-templates select="Screen"/>
		</TT_IssueTicketRS>
	</xsl:template>
	
	<xsl:template match="DocProdFareManipulation_10 | DocProdFareManipulation_29">
		<xsl:choose>
			<xsl:when test="TicketNumberData/ETicketNum/FirstTkNum!=''">
				<xsl:apply-templates select="TicketNumberData" mode="Success" />
			</xsl:when>
			<xsl:when test="ManualFareUpdate/ErrText/Text != ''">
				<xsl:apply-templates select="ManualFareUpdate" mode="Unsuccessful" />
			</xsl:when>
			<xsl:when test="HostApplicationError">
				<xsl:apply-templates select="HostApplicationError"/>
			</xsl:when>
      
      <!-- below given when block was not tehre in the local xsl file-->
			<xsl:when test="TransactionErrorCode">
				<xsl:apply-templates select="TransactionErrorCode"/>
			</xsl:when>
      <!--===========================================================-->
      
			<xsl:otherwise>
				<xsl:apply-templates select="Ticketing" mode="Unsuccessful" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="TicketNumberData" mode="Success">
		<Success />
		<xsl:if test="TextMsg/Txt != ''">
			<Warnings>
				<xsl:for-each select="TextMsg">
					<Warning><xsl:value-of select="Txt"/></Warning>
				</xsl:for-each>
			</Warnings>
		</xsl:if>
		<UniqueID>
			<xsl:attribute name="ID">
				<xsl:value-of select="../PNRBFManagement_53/PNRBFRetrieve/GenPNRInfo/RecLoc"/>
			</xsl:attribute>
		</UniqueID>
		<TicketingControl>
			<Type>OK</Type>
		</TicketingControl>
		<xsl:for-each select="ETicketNum[CurHistoryInd='C']">
			<Ticket>
				<xsl:attribute name="Type">
					<xsl:choose>
						<xsl:when test="TkType='E'">Electronic</xsl:when>
						<xsl:when test="TkType='P'">Paper</xsl:when>
						<xsl:when test="TkType='M'">MCO</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="Number">
					<xsl:value-of select="concat(AirV,FirstTkNum)"/>
				</xsl:attribute>
				<xsl:if test="Name!=''">
					<Name><xsl:value-of select="Name"/></Name>
				</xsl:if>
				<xsl:if test="Fare!=''">
					<Fare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="normalize-space(Fare)"/>
						</xsl:attribute>
						<xsl:attribute name="Currency">
							<xsl:value-of select="Currency"/>
						</xsl:attribute>
					</Fare>
				</xsl:if>
			</Ticket>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="PNRBFManagement_53">
		<xsl:apply-templates select="PNRBFRetrieve" mode="Unsuccessful"/>
		<xsl:apply-templates select="PNRBFSecondaryBldChg"/>
	</xsl:template>
	
	<xsl:template match="Ticketing | PNRBFRetrieve | ManualFareUpdate" mode="Unsuccessful">
		<Errors>
			<Error>
				<xsl:attribute name="Type">Galileo</xsl:attribute>
				<xsl:value-of select="ErrText/Text" />
			</Error>
		</Errors>
	</xsl:template>
	
	<xsl:template match="PNRBFSecondaryBldChg | HostApplicationError">
		<Errors>
			<Error>
				<xsl:attribute name="Type">Galileo</xsl:attribute>
				<xsl:value-of select="Text" />
			</Error>
		</Errors>
	</xsl:template>
  
	<!--below given template was not there in local xsl-->
	<xsl:template match="TransactionErrorCode">
		<Errors>
			<Error>
				<xsl:attribute name="Type">Galileo</xsl:attribute>
				<xsl:value-of select="'CANNOT ISSUE TICKET - UNKNOWN GALILEO ERROR'" />
			</Error>
		</Errors>
	</xsl:template>
	<!--================================================-->
  
  
	<xsl:template match="Screen">
		<xsl:variable name="lines">
			<xsl:for-each select="Line">
				<xsl:value-of select="."/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($lines,'FILED FARE 1 SENT TO PRINTER') or contains($lines,'ELECTRONIC TKT GENERATED')">
				<Success />
				<Warnings>
					<xsl:for-each select="Line">
						<Warning><xsl:value-of select="."/></Warning>
					</xsl:for-each>
				</Warnings>
				<UniqueID>
					<xsl:variable name="recloc"><xsl:value-of select="substring-after($lines,'RECORD LOCATOR: *')"/></xsl:variable>
					<xsl:attribute name="ID">
						<xsl:value-of select="substring($recloc,1,6)"/>
					</xsl:attribute>
				</UniqueID>
				<TicketingControl>
					<Type>OK</Type>
				</TicketingControl>
			</xsl:when>
			<xsl:otherwise>
				<Errors>
					<xsl:for-each select="Line">
						<Error><xsl:value-of select="."/></Error>
					</xsl:for-each>
				</Errors>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>
