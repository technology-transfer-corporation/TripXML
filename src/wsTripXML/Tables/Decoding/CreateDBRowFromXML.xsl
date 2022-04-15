<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text" version="1.0" encoding="UTF-8" />
	
	<xsl:template match="/">		
		<xsl:apply-templates select="AmadeusCars/CarCompany"/>	
	</xsl:template>
	
	<xsl:template match="CarCompany">
		<xsl:value-of select="concat('INSERT INTO [GlobalReservation].[dbo].[Car] ([Id],[Name]) VALUES (&amp;',Code,'&amp;,&amp;',Name,'&amp;)','&#10;')"/>
	</xsl:template>
</xsl:stylesheet>
