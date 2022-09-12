<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_RefundTicketRS.xsl 												-->
	<!-- ================================================================== -->
	<!-- Date: 24 Jul 2013 - Rastko															-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="DocRefund_ProcessRefundReply | Command_CrypticReply"/>
	</xsl:template>
	
	<xsl:template match="DocRefund_ProcessRefundReply | Command_CrypticReply">
		<TT_RefundTicketRS Version="1.001">
			<xsl:choose>
				<xsl:when test="longTextString">
					<xsl:choose>
						<xsl:when test="contains(longTextString/textStringDetails,'OK ETKT RECORD UPDATED SAC')">
							<Success/>
							<SAC>
								<xsl:value-of select="substring(substring-after(longTextString/textStringDetails,'OK ETKT RECORD UPDATED SAC- '),1,13)"/>
							</SAC>
						</xsl:when>
						<xsl:otherwise>
							<Errors>
								<Error>
									<xsl:attribute name="Type">Amadeus</xsl:attribute>
									<xsl:value-of select="longTextString/textStringDetails" />
								</Error>
							</Errors>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="applicationErrorGroup">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Amadeus</xsl:attribute>
							<xsl:value-of select="applicationErrorGroup/errorText/freeText" />
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success/>
					<SAC>
						<xsl:value-of select="sacNumber/referenceDetails/value"/>
					</SAC>
				</xsl:otherwise>
			</xsl:choose>
			
		</TT_RefundTicketRS >
	</xsl:template>

</xsl:stylesheet>