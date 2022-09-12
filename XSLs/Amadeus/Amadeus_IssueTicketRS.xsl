<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_IssueTicketRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 30 Oct 2008 - Rastko															-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="PoweredPNR_PNRReply" mode="ok"/>
		<xsl:variable name="resp">
			<xsl:choose>
				<xsl:when test="Cryptic_GetScreen_Reply">
					<xsl:value-of select="Cryptic_GetScreen_Reply/CAPI_Screen/Response"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="MessagesOnly_Reply/CAPI_Messages/Text"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($resp,'OK ')">
				<xsl:apply-templates select="MessagesOnly_Reply" mode="ok"/>
				<xsl:apply-templates select="Cryptic_GetScreen_Reply" mode="ok"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="MessagesOnly_Reply" mode="error"/>
				<xsl:apply-templates select="Cryptic_GetScreen_Reply" mode="error"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="MessagesOnly_Reply | Cryptic_GetScreen_Reply" mode="error">
		<TT_IssueTicketRS Version="1.001">
			<Errors>
				<xsl:choose>
					<xsl:when test="not(CAPI_Messages/Text) and not(CAPI_Screen/Response)">
						<Error> Amadeus Timeout</Error>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="CAPI_Messages" />
						<xsl:apply-templates select="CAPI_Screen/Response" />
					</xsl:otherwise>
				</xsl:choose>
				
			</Errors>
		</TT_IssueTicketRS>
	</xsl:template>
	
	<xsl:template match="PoweredPNR_PNRReply | MessagesOnly_Reply | Cryptic_GetScreen_Reply" mode="ok">
		<TT_IssueTicketRS Version="1.001">
			<Success/>
			<UniqueID>
				<xsl:attribute name="ID">
					<xsl:value-of select="pnrHeader/reservationInfo/reservation/controlNumber"/>
				</xsl:attribute>
			</UniqueID>
			<TicketingControl>
				<Type>OK</Type>
			</TicketingControl>
		</TT_IssueTicketRS>
	</xsl:template>
	
	<xsl:template match="CAPI_Messages">
		<Error>
			<xsl:attribute name="Code">
				<xsl:value-of select="ErrorCode" />
			</xsl:attribute>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	
	<xsl:template match="CAPI_Screen/Response">
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="." />
		</Error>
	</xsl:template>
	
</xsl:stylesheet>