<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ttVB="urn:ttVB" exclude-result-prefixes="msxsl ttVB" version="1.0">
<!-- ================================================================== -->
<!-- Galileo_VoidTicketRQ.xsl															-->
<!-- ================================================================== -->
<!-- Date: 02 Mar 2103 - Rastko														-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:variable name="gds" select="TT_VoidTicketRQ/POS/TPA_Extensions/Provider/Name"/>
	<xsl:template match="/">
		<xsl:apply-templates select="TT_VoidTicketRQ"/>
	</xsl:template>
	<xsl:template match="TT_VoidTicketRQ">
		<VoidTickets>
			<xsl:for-each select="Tickets/TicketNumber">
				<TicketVoid_2>
					<VoidTicketMods>
						<TicketNumberRange>
							<AirNumeric><xsl:value-of select="substring(.,1,3)"/></AirNumeric>
							<TkStockNum><xsl:value-of select="substring(.,5)"/></TkStockNum>
							<xsl:if test="$gds='Galileo'">
								<xsl:variable name="tkt"><xsl:value-of select="translate(.,'-','')"/></xsl:variable>
								<TkNumCheckDigit>
									<xsl:value-of select="$tkt mod 7"/>
								</TkNumCheckDigit>
							</xsl:if>
						</TicketNumberRange>
					</VoidTicketMods>
				</TicketVoid_2>
			</xsl:for-each>
		</VoidTickets>
	</xsl:template>
</xsl:stylesheet>
