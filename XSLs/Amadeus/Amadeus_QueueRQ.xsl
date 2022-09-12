<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Amadeus_QueueRQ.xsl 															-->
<!-- ================================================================== -->
<!-- Date: 30 Jul - new file - Rastko													-->
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
		<PoweredQueue_PlacePNR> 
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
    			</targetDetails>
    			<recordLocator>
    				<reservation>
    					<controlNumber><xsl:value-of select="UniqueID/@ID"/></controlNumber>
    				</reservation>
    			</recordLocator>
		</PoweredQueue_PlacePNR> 
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
		<PoweredQueue_RemoveItem>
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
				<QueueNumber>
					<queueDetails>
						<number><xsl:value-of select="@Number"/></number>
					</queueDetails>
				</QueueNumber>
			</targetDetails>
		</PoweredQueue_RemoveItem>	
	</xsl:template>
	
	<xsl:template match="Move">
		<PoweredQueue_MoveItem>
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
				<QueueNumber>
					<queueDetails>
						<number><xsl:value-of select="FromQueue/@Number"/></number>
					</queueDetails>
				</QueueNumber>
			</targetDetails>
			<targetDetails>
				<targetOffice>
					<sourceType>
						<sourceQualifier1>3</sourceQualifier1>
					</sourceType>
				</targetOffice>
				<QueueNumber>
					<queueDetails>
						<number><xsl:value-of select="ToQueue/@Number"/></number>
					</queueDetails>
				</QueueNumber>
			</targetDetails>
			<numberOfPNRs>
				<quantityDetails>
					<numberOfUnit><xsl:value-of select="@ItemsQuantity"/></numberOfUnit>
				</quantityDetails>
			</numberOfPNRs>
		</PoweredQueue_MoveItem>
	</xsl:template>
	
	<xsl:template match="ListQueue">
		<PoweredQueue_List> 
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
		</PoweredQueue_List>
	</xsl:template>
	
	<xsl:template match="CountQueue">
		<PoweredQueue_CountTotal>
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
		</PoweredQueue_CountTotal>
	</xsl:template>
	
</xsl:stylesheet>
