<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- 
================================================================== 
 AmadeusWS_DeleteVirtualCardRS.xsl									
================================================================== 
 Date: 03 May 2021 - Kobelev - Added Conversation ID to schema	
 Date: 04 Feb 2019 - Kobelev - TripXML Refectoring				
 Date: 16 Jan 2019 - Rastko - new file											
================================================================== 
-->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
  
	<xsl:template match="/">
		<xsl:apply-templates select="AMA_PAY_DeleteVirtualCardRS"/>
	</xsl:template>
  
	<xsl:template match="AMA_PAY_DeleteVirtualCardRS">
		<PAY_DeleteVirtualCardRS>
		  <xsl:if test="Failure">
		    <Errors>
		      <xsl:apply-templates select="Failure"/>
		    </Errors>
		  </xsl:if>
		  <xsl:if test="Warnings">
		    <xsl:apply-templates select="Warnings"/>
		  </xsl:if>
		  <xsl:if test="Success">
		    <xsl:apply-templates select="Success"/>
		  </xsl:if>
      <xsl:if test="ConversationID!=''">
        <ConversationID>
          <xsl:value-of select="ConversationID"/>
        </ConversationID>
      </xsl:if>
		</PAY_DeleteVirtualCardRS>
	</xsl:template>

  <xsl:template match="Failure">
    <xsl:for-each select="Errors/Errors/Error">
      <Error>
        <xsl:attribute name="Type">
          <xsl:value-of select="@Type"/>
        </xsl:attribute>
        <xsl:attribute name="ShortText">
          <xsl:value-of select="@ShortText"/>
        </xsl:attribute>
        <xsl:attribute name="Code">
          <xsl:value-of select="@Code"/>
        </xsl:attribute>
        <xsl:value-of select="@ShortText"/>
      </Error>
    </xsl:for-each>
  </xsl:template>
  
  <xsl:template match="Warnings">
    <xsl:copy-of select="Warnings"/>
  </xsl:template>
  
  <xsl:template match="Success">
    <Success />
    <xsl:if test="Warnings">
      <xsl:apply-templates select="Warnings"/>
    </xsl:if>
    <xsl:if test="References">
      <xsl:copy-of select="References"/>
    </xsl:if>
  </xsl:template>
  
</xsl:stylesheet>
