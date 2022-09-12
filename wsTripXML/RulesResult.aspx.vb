Imports System
Imports System.Xml
Partial Public Class RulesResult
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not (Session("RulesNodes") Is Nothing) Then
                Dim nodeList As XmlNodeList = CType(Session("RulesNodes"), XmlNodeList)
                RPTHeader.DataSource = nodeList
                RPTHeader.DataBind()
                Session("RulesNodes") = Nothing
            Else

            End If




        Catch ex As Exception

        End Try

    End Sub
    Protected Sub RPTHeader_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTHeader.ItemDataBound

        Dim node As XmlNode
        node = CType(e.Item.DataItem, XmlNode)

        Dim lbl As Label = CType(e.Item.FindControl("lblTitle"), Label)
        lbl.Text = node.Attributes("SubTitle").Value
        Dim ndList As XmlNodeList = node.SelectNodes("Paragraph")



        Dim tb As Table = CType(e.Item.FindControl("tb"), Table)
        For Each oNode In ndList

            Dim row As TableRow = New TableRow()
            Dim cell As TableCell = New TableCell()
            cell.Text = oNode.SelectSingleNode("Text").InnerText
            row.Cells.Add(cell)
            tb.Rows.Add(row)
        Next
        'RPTDetails = CType(e.Item.FindControl("RPTDetails"), Repeater)



        'RPTDetails.DataSource = ndList
        'RPTDetails.DataBind()





    End Sub
End Class