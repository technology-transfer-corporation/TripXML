<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="xml" omit-xml-declaration="yes"  indent="yes"/>
	<xsl:template match="/">
		<WSDLSchema>
			<xsl:apply-templates select="schema/import">
				<xsl:sort order="ascending" data-type="text" select="@schemaLocation"/>
			</xsl:apply-templates>
		</WSDLSchema>
	</xsl:template>
	<xsl:template match="import">
		<Message>
			<xsl:attribute name="Name">
				<xsl:value-of select="substring-before(@schemaLocation,'_')"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="substring-before(substring-after(@schemaLocation,'_'),'_')"/>
			</xsl:attribute>
			<xsl:attribute name="Version">
				<xsl:value-of select="substring-after(substring-after(@namespace,'//'),'/')"/>
			</xsl:attribute>
		</Message>
	</xsl:template>
	<!--xsl:template match="import">
		<DIM>
			<xsl:text>Dim </xsl:text>
			<xsl:value-of select="substring-before(@schemaLocation,'_')"/>
			<xsl:text>_</xsl:text>
			<xsl:value-of select="substring-before(substring-after(@schemaLocation,'_'),'_')"/>
			<xsl:text> As String</xsl:text>
		</DIM>
	</xsl:template-->
	<!--xsl:template match="import">
		<DIM>
			<xsl:variable name="m">
				<xsl:value-of select="substring-before(@schemaLocation,'_')"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="substring-before(substring-after(@schemaLocation,'_'),'_')"/>
			</xsl:variable>
			wsNode = oNodePCC.SelectSingleNode(&quot;WSDLSchema/Message[@Name = &apos;<xsl:value-of select="$m"/>&apos;]&quot;)

			If Not wsNode Is Nothing Then
			    .AmadeusWSSchema.<xsl:value-of select="$m"/> = wsNode.Attributes(&quot;Version&quot;).Value
			Else
			    .AmadeusWSSchema.<xsl:value-of select="$m"/> = &quot;&quot;
			End If
		</DIM>
	</xsl:template-->
</xsl:stylesheet>
