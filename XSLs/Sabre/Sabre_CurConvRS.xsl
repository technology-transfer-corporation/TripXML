<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

<xsl:output method="xml" omit-xml-declaration="yes"/>
<xsl:template match = "/">
	<OTA_CurConvRS>
		<Success/>
		<xsl:apply-templates select="SabreCommandLLSRS/Response"/>
	</OTA_CurConvRS>
</xsl:template>

<xsl:template match="Response">
	<xsl:variable name="resp"><xsl:value-of select="substring(string(.),188)"/></xsl:variable>
	<xsl:variable name="from"><xsl:value-of select="substring-before(substring($resp,5,10),' ')"/></xsl:variable>
	<xsl:variable name="to"><xsl:value-of select="substring-before(substring($resp,45,10),' ')"/></xsl:variable>
	<xsl:variable name="tofrom"><xsl:value-of select="substring(string($to div $from),1,6)"/></xsl:variable>
	<Conversion>
		<From>
			<xsl:attribute name="Amount"><xsl:value-of select="$from"/></xsl:attribute>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="substring($resp,1,3)"/></xsl:attribute>
		</From>
		<To>
			<xsl:attribute name="Amount"><xsl:value-of select="$to"/></xsl:attribute>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="substring($resp,41,3)"/></xsl:attribute>
		</To>
		<ConversionRate><xsl:value-of select="$tofrom"/></ConversionRate> 
		<Rounding><xsl:value-of select="string-length(substring-after($tofrom,'.')) + 1"/></Rounding> 
	</Conversion>
</xsl:template>

</xsl:stylesheet>




