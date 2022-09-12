<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<TravelBuildRebook>
			<xsl:variable name="err"><xsl:value-of select="count(TravelBuildRebook/Cancel/Segment[@Err])"/></xsl:variable>
			<xsl:variable name="seg"><xsl:value-of select="count(TravelBuildRebook/Cancel/Segment)"/></xsl:variable>
			<xsl:if test="$err != $seg">
				<PoweredPNR_Cancel>
					<pnrActions>
						<optionCode>0</optionCode>
					</pnrActions>
					<cancelElements>
						<entryType>E</entryType>
						<xsl:apply-templates select="TravelBuildRebook/Cancel/Segment[not(@Err)]"/>
					</cancelElements>
				</PoweredPNR_Cancel>
			</xsl:if>
			<PoweredPNR_AddMultiElements>
				<pnrActions>
					<optionCode>0</optionCode>
				</pnrActions>
				<xsl:for-each select="TravelBuildRebook/Cancel/Segment">
					<xsl:variable name="tattoo">
						<xsl:value-of select="."/>
					</xsl:variable>
					<xsl:apply-templates select="//TravelBuildRebook/PoweredPNR_PNRReply/originDestinationDetails/itineraryInfo[elementManagementItinerary/reference/number = $tattoo]"/>
				</xsl:for-each>
			</PoweredPNR_AddMultiElements>
		</TravelBuildRebook>
	</xsl:template>
	
	<xsl:template match="Segment">
		<element>
			<identifier>ST</identifier>
			<number>
				<xsl:value-of select="."/>
			</number>
		</element>
	</xsl:template>
	
	<xsl:template match="itineraryInfo">
		<originDestinationDetails>
			<originDestination>
				<origin><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></origin>
				<destination><xsl:value-of select="travelProduct/offpointDetail/cityCode"/></destination>
			</originDestination>
			<itineraryInfo>
				<elementManagementItinerary>
					<segmentName>AIR</segmentName>
				</elementManagementItinerary>
				<airAuxItinerary>
					<travelProduct>
						<product>
							<depDate><xsl:value-of select="travelProduct/product/depDate"/></depDate>
							<depTime><xsl:value-of select="travelProduct/product/depTime"/></depTime>
							<arrDate><xsl:value-of select="travelProduct/product/arrDate"/></arrDate>
							<arrTime><xsl:value-of select="travelProduct/product/arrTime"/></arrTime>
						</product>
						<boardpointDetail>
							<cityCode><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></cityCode>
						</boardpointDetail>
						<offpointDetail>
							<cityCode><xsl:value-of select="travelProduct/offpointDetail/cityCode"/></cityCode>
						</offpointDetail>
						<company>
							<identification><xsl:value-of select="travelProduct/companyDetail/identification"/></identification>
						</company>
						<productDetails>
							<identification><xsl:value-of select="travelProduct/productDetails/identification"/></identification>
							<classOfService><xsl:value-of select="travelProduct/productDetails/classOfService"/></classOfService>
						</productDetails>
					</travelProduct>
					<messageAction>
						<business>
							<function>1</function>
						</business>
					</messageAction>
					<relatedProduct>
						<quantity><xsl:value-of select="relatedProduct/quantity"/></quantity>
						<status>NN</status>
					</relatedProduct>
					<selectionDetailsAir>
						<selection>
							<option>P10</option>
						</selection>
					</selectionDetailsAir>
				</airAuxItinerary>
			</itineraryInfo>
		</originDestinationDetails>
	</xsl:template>
</xsl:stylesheet>
