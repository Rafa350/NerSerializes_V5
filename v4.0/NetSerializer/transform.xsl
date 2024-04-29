<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="xml" indent="yes" encoding="utf-8"/>
	
	<xsl:template match="/DOCUMENT">
        <xsl:element name="document">
            <xsl:attribute name="version">400</xsl:attribute>
            <xsl:attribute name="encodeStrings">true</xsl:attribute>
            <xsl:attribute name="useNames"><xsl:value-of select="@useValueNames"/></xsl:attribute>
            <xsl:apply-templates select="DATA"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="DATA">
        <xsl:element name="data">
            <xsl:attribute name="version"><xsl:value-of select="@version"/></xsl:attribute>
            <xsl:apply-templates select="OBJECT"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="OBJECT">
        <xsl:element name="object">
            <xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute>
            <xsl:attribute name="type"><xsl:value-of select="@type"/></xsl:attribute>
            <xsl:attribute name="id"><xsl:value-of select="@id"/></xsl:attribute>
            <xsl:apply-templates select="*"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="REFERENCE">
        <xsl:choose>
            <xsl:when test="@refid!='nul'">
                <xsl:element name="reference">
                    <xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute>
                    <xsl:attribute name="id"><xsl:value-of select="@refid"/></xsl:attribute>
                </xsl:element>
            </xsl:when>
            <xsl:otherwise>
                <xsl:element name="null">
                    <xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute>
                </xsl:element>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template match="VALUE">
        <xsl:element name="value">
            <xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute>
            <xsl:value-of select="."/>            
        </xsl:element>
    </xsl:template>
    
    
 </xsl:stylesheet>
 
 