<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_QueueRQ.xsl 														-->
<!-- ================================================================== -->
<!-- Date: 02 Jun 2011 - Rastko - added mapping of queue date range in queue place	-->
<!-- Date: 10 Aug 2009 - Rastko														-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_QueueRQ" />
	</xsl:template>
	
	<xsl:template match="OTA_QueueRQ">
		<xsl:apply-templates select="ListQueue"/>
		<xsl:apply-templates select="CountQueue"/>
		<xsl:apply-templates select="RemoveQueue"/>
		<xsl:apply-templates select="BounceQueue"/>
		<xsl:apply-templates select="Move"/>
		<xsl:apply-templates select="PlaceQueue"/>
		<!--xsl:apply-templates select="Clean"/-->
	</xsl:template>
	
	<xsl:template match="PlaceQueue">
		<Queue_PlacePNR> 
    			<placementOption>
				<selectionDetails>
					<option>QEQ</option>
				</selectionDetails> 
    			</placementOption>
    			<targetDetails>
    				<xsl:if test="@PseudoCityCode != ''">
					<targetOffice> 
						<sourceType> 
							<sourceQualifier1>4</sourceQualifier1> 
						</sourceType> 
						<originatorDetails> 
							<inHouseIdentification1><xsl:value-of select="@PseudoCityCode"/></inHouseIdentification1> 
						</originatorDetails> 
					</targetOffice> 
				</xsl:if>
    				<xsl:if test="@Number != ''">
					<queueNumber> 
						<queueDetails> 
							<number><xsl:value-of select="@Number"/></number> 
						</queueDetails> 
					</queueNumber>
				</xsl:if>
				<xsl:if test="@Category != '' or @Name != ''">
					<categoryDetails> 
						<subQueueInfoDetails>
							<xsl:choose>
								<xsl:when test="@Category != ''">
									<identificationType>C</identificationType> 
							    		<itemNumber>
							    			<xsl:choose>
							    				<xsl:when test="contains(@Category,'D')">
							    					<xsl:value-of select="substring-before(@Category,'D')"/>
							    				</xsl:when>
							    				<xsl:otherwise>
							    					<xsl:value-of select="@Category"/>
							    				</xsl:otherwise>
										</xsl:choose>
							    		</itemNumber>
							    </xsl:when>
							    <xsl:otherwise>
							    		<identificationType>N</identificationType> 
							    		<itemDescription><xsl:value-of select="@Name"/></itemDescription> 
							    </xsl:otherwise>
							</xsl:choose>
						</subQueueInfoDetails> 
					</categoryDetails>	
					<xsl:if test="contains(@Category,'D')">
						<placementDate>
							<timeMode><xsl:value-of select="substring-after(@Category,'D')"/></timeMode>
						</placementDate>
					</xsl:if>				
				</xsl:if>
    			</targetDetails>
    			<recordLocator>
    				<reservation>
    					<controlNumber><xsl:value-of select="UniqueID/@ID"/></controlNumber>
    				</reservation>
    			</recordLocator>
		</Queue_PlacePNR> 
	</xsl:template>
	
	<!--xsl:template match="Clean">
	</xsl:template-->
	
	<xsl:template match="BounceQueue">	   
		<xsl:choose>
			<xsl:when test="@Action='R'">
				<Cryptic_GetScreen_Query>
					<Command>QN</Command>
				</Cryptic_GetScreen_Query> 
			</xsl:when>
			<xsl:when test="@Action='I'">
				<Cryptic_GetScreen_Query>
					<Command>IG</Command>
				</Cryptic_GetScreen_Query>  
			</xsl:when>
			<xsl:when test="@Action='E'">
				<Cryptic_GetScreen_Query>
					<Command>ER</Command>
				</Cryptic_GetScreen_Query>
			</xsl:when>
		</xsl:choose> 
   	</xsl:template>

	<xsl:template match="RemoveQueue">
		<Queue_RemoveItem>
			<removalOption>
				<selectionDetails>
					<option>QR</option>
				</selectionDetails>
			</removalOption>
			<targetDetails>
				<targetOffice>
					<sourceType>
						<sourceQualifier1>3</sourceQualifier1>
					</sourceType>
				</targetOffice>
				<queueNumber>
					<queueDetails>
						<number><xsl:value-of select="@Number"/></number>
					</queueDetails>
				</queueNumber>
			</targetDetails>
		</Queue_RemoveItem>	
	</xsl:template>
	
	<xsl:template match="Move">
		<Queue_MoveItem>
			<placementOption>
				<selectionDetails>
					<option>QBD</option>
				</selectionDetails>
			</placementOption>
			<targetDetails>
				<targetOffice>
					<sourceType>
						<sourceQualifier1>3</sourceQualifier1>
					</sourceType>
				</targetOffice>
				<queueNumber>
					<queueDetails>
						<number><xsl:value-of select="FromQueue/@Number"/></number>
					</queueDetails>
				</queueNumber>
				<categoryDetails>
					<subQueueInfoDetails>
						<identificationType>C</identificationType>
						<itemNumber>0</itemNumber>
					</subQueueInfoDetails>
				</categoryDetails>
				<placementDate>
					<timeMode>1</timeMode>
				</placementDate>
			</targetDetails>
			<targetDetails>
				<targetOffice>
					<sourceType>
						<sourceQualifier1>3</sourceQualifier1>
					</sourceType>
				</targetOffice>
				<queueNumber>
					<queueDetails>
						<number><xsl:value-of select="ToQueue/@Number"/></number>
					</queueDetails>
				</queueNumber>
				<categoryDetails>
					<subQueueInfoDetails>
						<identificationType>C</identificationType>
						<itemNumber>0</itemNumber>
					</subQueueInfoDetails>
				</categoryDetails>
				<placementDate>
					<timeMode>1</timeMode>
				</placementDate>
			</targetDetails>
			<numberOfPNRs>
				<quantityDetails>
					<numberOfUnit><xsl:value-of select="@ItemsQuantity"/></numberOfUnit>
				</quantityDetails>
			</numberOfPNRs>
		</Queue_MoveItem>
	</xsl:template>
	
	<xsl:template match="ListQueue">
		<Queue_List> 
			<xsl:if test="@PseudoCityCode != ''">
				<targetOffice> 
					<sourceType> 
						<sourceQualifier1>4</sourceQualifier1> 
					</sourceType> 
					<originatorDetails> 
						<inHouseIdentification1><xsl:value-of select="@PseudoCityCode"/></inHouseIdentification1> 
					</originatorDetails> 
				</targetOffice> 
			</xsl:if>
			<xsl:if test="@Number != ''">
				<queueNumber> 
					<queueDetails> 
						<number><xsl:value-of select="@Number"/></number> 
					</queueDetails> 
				</queueNumber>
			</xsl:if>
			<xsl:if test="@Category != '' or @Name != ''">
				<categoryDetails> 
					<subQueueInfoDetails>
						<xsl:choose>
							<xsl:when test="@Category != ''">
								<identificationType>C</identificationType> 
						    		<itemNumber><xsl:value-of select="@Category"/></itemNumber>
						    </xsl:when>
						    <xsl:otherwise>
						    		<identificationType>N</identificationType> 
						    		<itemDescription><xsl:value-of select="@Name"/></itemDescription> 
						    </xsl:otherwise>
						</xsl:choose>
					</subQueueInfoDetails> 
				</categoryDetails>					
			</xsl:if>
			<scanRange>
				<rangeQualifier>701</rangeQualifier>
				<rangeDetails>
					<min>1</min>
					<max>200</max>
				</rangeDetails>
			</scanRange>
		</Queue_List>
	</xsl:template>
	
	<xsl:template match="CountQueue">
		<Queue_CountTotal>
			<queueingOptions>
				<selectionDetails>
					<option>
						<xsl:choose>
							<xsl:when test="@Summary = 'true'">QTQ</xsl:when>
							<xsl:otherwise>QT</xsl:otherwise>
						</xsl:choose>
					</option>
				</selectionDetails> 
			</queueingOptions> 
			<xsl:if test="@PseudoCityCode != ''">
				<targetOffice> 
					<sourceType> 
						<sourceQualifier1>4</sourceQualifier1> 
					</sourceType> 
					<originatorDetails> 
						<inHouseIdentification1><xsl:value-of select="@PseudoCityCode"/></inHouseIdentification1> 
					</originatorDetails> 
				</targetOffice> 
			</xsl:if>
			<xsl:choose>
				<xsl:when test="@Number != ''">
					<QueueNumber> 
						<queueDetails> 
							<number><xsl:value-of select="@Number"/></number> 
						</queueDetails> 
					</QueueNumber>
				</xsl:when>
				<xsl:when test="@Name != ''">
					<categorySelection> 
						<subQueueInfoDetails> 
							<identificationType>N</identificationType> 
							<itemDescription><xsl:value-of select="@Name"/></itemDescription> 
						</subQueueInfoDetails> 
					</categorySelection>					
				</xsl:when>
			</xsl:choose>
		</Queue_CountTotal>
	</xsl:template>
	
</xsl:stylesheet>
