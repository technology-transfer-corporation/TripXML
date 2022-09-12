<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- v03_TravelFusion_PNRReadRQ.xsl 													-->
<!-- ================================================================== -->
<!-- Date: 09 Jun 2012 - Rastko - new file													-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<CommandList>
			<GetBookingDetails>
				<XmlLoginId>***XMLLogin***</XmlLoginId>
				<LoginId>***LoginID***</LoginId>
				<TFBookingReference><xsl:value-of select="OTA_ReadRQ/UniqueID/@ID" /></TFBookingReference>
			</GetBookingDetails>
		</CommandList>
	</xsl:template>
</xsl:stylesheet>
