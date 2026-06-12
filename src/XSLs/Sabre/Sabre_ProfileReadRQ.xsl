<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_ProfileReadRQ.xsl 													-->
	<!-- ================================================================== -->
	<!-- Date: 02 Aug 2016 - Rastko - added mapping of owning office id				-->
	<!-- Date: 11 Jul 2016 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
    <Sabre_OTA_ProfileReadRQ xmlns="http://www.sabre.com/eps/schemas" Version="1.0">
      <Profile>
        <TPA_Identity ClientCode="TN" ClientContextCode="TMP" >
          <xsl:attribute name="DomainID">
                <xsl:value-of select="OTA_ProfileReadRQ/POS/Source/@PseudoCityCode"/>
          </xsl:attribute>
          <xsl:attribute name="UniqueID">
            <xsl:value-of select="OTA_ProfileReadRQ/UniqueID/@ID"/>
          </xsl:attribute> 
        </TPA_Identity>
      </Profile>
    </Sabre_OTA_ProfileReadRQ>
	</xsl:template>
</xsl:stylesheet>
