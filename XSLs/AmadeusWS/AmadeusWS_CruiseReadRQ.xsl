<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseReadRQ.xsl 	   												       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<PNR_Retrieve>
			<settings>
				<options>
					<optionCode>51</optionCode>
				</options>
			</settings>
			<retrievalFacts>
				<retrieve>
					<type>2</type>
					<office></office>
				</retrieve>
				<reservationOrProfileIdentifier>
					<reservation>
						<controlNumber>
							<xsl:value-of select="OTA_CruiseReadRQ/UniqueID/@ID" />
						</controlNumber>
					</reservation>
				</reservationOrProfileIdentifier>
			</retrievalFacts>
		</PNR_Retrieve>
	</xsl:template>
</xsl:stylesheet>
