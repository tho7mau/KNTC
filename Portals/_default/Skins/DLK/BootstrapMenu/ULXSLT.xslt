<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/*">
    <xsl:apply-templates select="root" />
  </xsl:template>
  <xsl:template match="root">
    <ul class="sidebar-menu">
      <xsl:apply-templates select="node">
        <xsl:with-param name="level" select="0"/>
      </xsl:apply-templates>
    </ul>
  </xsl:template>
  <xsl:template match="node">
    <xsl:param name="level" />
    <xsl:choose>
      <xsl:when test="$level=0">
        <li>
          <xsl:attribute name="class">
            <xsl:if test="@breadcrumb = 1">active </xsl:if>
            <xsl:if test="node">
              <xsl:text>&#32;treeview</xsl:text>
            </xsl:if>
          </xsl:attribute>

          <xsl:choose>
            <xsl:when test="@enabled = 1">
              <a href="{@url}">
                <xsl:attribute name="class">
                  <xsl:if test="node">
                    <xsl:text>dropdown-toggle</xsl:text>
                  </xsl:if>
                  
                </xsl:attribute>
                <xsl:if test="node">
                  <xsl:attribute name="data-toggle">treeview</xsl:attribute>
                </xsl:if>
                <xsl:if test="string-length(@icon) &gt; 0">
                  <img src="{@icon}" />
                </xsl:if>
                <span>
                  <xsl:value-of select="@text" />
                </span>
                <xsl:if test="node">
                  <i class="fa fa-angle-left pull-right"></i>
                </xsl:if>
              </a>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="@text" />
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="node">
            <ul>
              <xsl:attribute name="class">
                <xsl:text>treeview-menu level </xsl:text>
                <xsl:value-of select="$level+1"/>
              </xsl:attribute>
              <xsl:apply-templates select="node">
                <xsl:with-param name="level" select="$level + 1" />
              </xsl:apply-templates>
            </ul>
          </xsl:if>
        </li>
      </xsl:when>
      <xsl:otherwise>

        <li>
          <xsl:attribute name="class">
            <xsl:if test="@breadcrumb = 1">active</xsl:if>
            <xsl:if test="node">
              <xsl:text>&#32;treeview level</xsl:text>
              <xsl:value-of select="$level+1"/>
            </xsl:if>
          </xsl:attribute>
          <xsl:choose>
            <xsl:when test="@enabled = 1">
              <a href="{@url}">
                <xsl:attribute name="class">
                  <xsl:if test="node">
                    <xsl:text>dropdown-toggle </xsl:text>
                  </xsl:if>
                </xsl:attribute>
                <xsl:if test="node">
                  <xsl:attribute name="data-toggle">treeview</xsl:attribute>
                </xsl:if>
                <xsl:if test="string-length(@icon) &gt; 0">
                  <img src="{@icon}" class="icon"/>
                </xsl:if>
                <xsl:value-of select=" @text" />
                <xsl:if test="node">
                  <i class="fa fa-angle-left pull-right"></i>
                </xsl:if>
              </a>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="@text" />
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="node">
            <ul>
              <xsl:attribute name="class">
                <xsl:text>treeview-menu level</xsl:text>
                <xsl:value-of select="$level+1"/>
              </xsl:attribute>
              <xsl:apply-templates select="node">
                <xsl:with-param name="level" select="$level + 1" />
              </xsl:apply-templates>
            </ul>
          </xsl:if>
        </li>

      </xsl:otherwise>

    </xsl:choose>


  </xsl:template>
</xsl:stylesheet>