<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Sabre_IssueTicketRS.xsl 															-->
	<!-- ================================================================== -->
  <!-- Date: 19 May 2014 - Suraj Chaged the code to read the e ticket no				-->
	<!-- Date: 27 Mar 2014 - added support for designate printer errors					-->
	<!-- Date: 05 Mar 2014 - corrected mapping of ticket control type					-->
	<!-- Date: 21 Feb 2014 - indicate a ticket when it is void								-->
	<!-- Date: 29 Jan 2014 - added optional ConversationID mapping					-->
	<!-- Date: 09 May 2012- Kasun done the mapping of ETicket no					-->
	<!-- Date: 10 Dec 2010 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="AirTicketRS"/>
	</xsl:template>
	<xsl:template match="AirTicketRS">
		<TT_IssueTicketRS Version="1.001">
			<xsl:choose>
				<xsl:when test="Errors">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:value-of select="Errors/Error/ErrorInfo/Message"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:when test="DesignatePrinterRS/Errors">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:value-of select="DesignatePrinterRS/Errors/Error/ErrorInfo/Message"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success/>
					<UniqueID>
						<xsl:attribute name="ID"><xsl:value-of select="pnrHeader/reservationInfo/reservation/controlNumber"/></xsl:attribute>
					</UniqueID>
					<TicketingControl Type="OK"/>
					<xsl:if test="IssuedTickets/IssuedTicket!=''">
						<xsl:apply-templates select="IssuedTickets/IssuedTicket"/>
					</xsl:if>
					<!--<Ticket>
						<xsl:attribute name="Number">
						</xsl:attribute>						
						<xsl:if test="TicketTotal/@Amount!=''">
							<Fare>
								<xsl:attribute name="Amount"><xsl:value-of select="TicketTotal/@Amount"/></xsl:attribute>
								<xsl:attribute name="DecimalPlaces"><xsl:value-of select="TicketTotal/@DecimalPlaces"/></xsl:attribute>
							</Fare>
						</xsl:if>
					</Ticket>-->
				</xsl:otherwise>
			</xsl:choose>
			<xsl:if test="ConversationID!=''">
				<ConversationID>
					<xsl:value-of select="ConversationID"/>
				</ConversationID>
			</xsl:if>
		</TT_IssueTicketRS>
	</xsl:template>
	<xsl:template match="IssuedTicket">
		<Ticket>
			<xsl:if test="substring(.,1,2)='TV'">
				<xsl:attribute name="Type">Void</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Number">
				<xsl:variable name="tn">
          <!--xsl:value-of select="substring-before(substring-after(.,'TE'),'-')"/-->
          <xsl:value-of select="substring(substring-after(.,'TE'),0,15)"/>
				</xsl:variable>
				<xsl:value-of select="$tn"/>
			</xsl:attribute>
		</Ticket>
	</xsl:template>
</xsl:stylesheet>
