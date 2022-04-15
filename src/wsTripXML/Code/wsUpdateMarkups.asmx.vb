Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports TripXMLMain
Imports TripXMLMain.modCore
Imports System.Xml
Imports System.IO
Imports System.Configuration

Namespace wsTravelTalk

    ' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    ' <System.Web.Script.Services.ScriptService()> _
    <System.Web.Services.WebService(Namespace:="http://tripxml.com/wsUpdateMarkups")> _
    <System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <ToolboxItem(False)> _
    Public Class wsUpdateMarkups
        Inherits System.Web.Services.WebService
        Private sb As StringBuilder = New StringBuilder()

        Private Function ServiceRequest(ByVal strRequest As String) As String
            Dim strResponse As String = ""
            Dim markUp As FileInfo
            Dim writer As XmlTextWriter = Nothing
            Dim xmlDoc As XmlDocument
            Dim oDoc As XmlDocument
            Dim oRoot As XmlElement
            Dim oNode As XmlNode
            Dim newstrRequest As String = ""

            Try
                If Trace Then CoreLib.SendTrace("", "wsUpdateMarkups", "============= Request ============= ", strRequest, String.Empty)

                oDoc = New XmlDocument()
                oDoc.LoadXml(strRequest)
                oRoot = oDoc.DocumentElement

                For Each oNode In oRoot.SelectNodes("Promotion")
                    Dim piNode As XmlNode = oNode.SelectSingleNode("Id")
                    Dim childPINode As XmlNode = oDoc.CreateNode("element", "PromotionId", "")
                    childPINode.InnerText = piNode.InnerText
                    oNode.ReplaceChild(childPINode, piNode)

                    'Dim childAMNode As XmlNode = oDoc.CreateNode("element", "AppliedMarkup", "")
                    'childAMNode.InnerText = "Base"
                    'oNode.AppendChild(childAMNode)

                    Dim ftNode As XmlNode = oNode.SelectSingleNode("FareTypes")
                    Dim childFTNode As XmlNode
                    childFTNode = oDoc.CreateNode("element", "FareType", "")
                    If Not ftNode.SelectSingleNode("Id") Is Nothing Then
                        childFTNode.InnerText = ftNode.SelectSingleNode("Id").InnerText
                    Else
                        childFTNode.InnerText = ""
                    End If
                    oNode.ReplaceChild(childFTNode, ftNode)

                    Dim scNode As XmlNode = oNode.SelectSingleNode("SupplierCodes")

                    If scNode.SelectNodes("Id").Count > 0 Then

                        Dim idNode As XmlNode
                        Dim i As Integer = 0
                        Dim firstNode As XmlNode
                        firstNode = oDoc.CreateNode("element", "SupplierCode", "")
                        firstNode.InnerText = scNode.SelectNodes("Id").Item(0).InnerText

                        For Each idNode In scNode.SelectNodes("Id")
                            Dim childNode As XmlNode
                            childNode = oDoc.CreateNode("element", "SupplierCode", "")
                            childNode.InnerText = idNode.InnerText

                            If i > 0 Then
                                Dim pNode As XmlNode = oDoc.CreateNode("element", "Promotion", "")
                                pNode.InnerXml = oNode.InnerXml
                                pNode.ReplaceChild(childNode, pNode.SelectSingleNode("SupplierCodes"))
                                oRoot.InsertAfter(pNode, oNode)
                            End If
                            i = i + 1
                        Next

                        oNode.ReplaceChild(firstNode, scNode)
                    Else
                        Dim childNode As XmlNode
                        childNode = oDoc.CreateNode("element", "SupplierCode", "")
                        childNode.InnerText = ""
                        oNode.ReplaceChild(childNode, scNode)
                    End If
                Next

                strRequest = oDoc.OuterXml

                xmlDoc = New XmlDocument()
                markUp = New FileInfo(ConfigurationManager.AppSettings("TripXMLFolder") + "\Xsl\Aggregation\Markups.xml")
                writer = New XmlTextWriter(markUp.FullName, Nothing)
                xmlDoc.LoadXml(strRequest)
                xmlDoc.Save(writer)
                strResponse = "Success"
            Catch ex As Exception
                strResponse = ex.Message
            Finally
                If Not writer Is Nothing Then
                    writer.Close()
                End If
                If Trace Then CoreLib.SendTrace("", "wsUpdateMarkups", "============= Response ============= ", strResponse, String.Empty)
            End Try

            Return strResponse
            sb = Nothing
        End Function

        <WebMethod(Description:="Update Markups.")> _
        Public Function UpdateMarkups(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest)
        End Function

    End Class
End Namespace